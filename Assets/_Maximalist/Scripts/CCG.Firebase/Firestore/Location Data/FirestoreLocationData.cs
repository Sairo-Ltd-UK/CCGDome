// Assets/Scripts/Firebase/Modules/FirestoreLocationData.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

namespace CCG.Firebase
{
    /// <summary>
    /// Stores location entries in LocationData/{UID} as an array field "entries".
    /// Each entry is a map: { locationName: string, time: Timestamp }.
    /// </summary>
    public sealed class FirestoreLocationData
    {
        private readonly FirebaseFirestore _db;
        private readonly Func<string> _getUid;

        private const string RootCollection = "LocationData";
        private const string EntriesField   = "entries";

        public FirestoreLocationData(FirebaseFirestore db, Func<string> getUid)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _getUid = getUid ?? throw new ArgumentNullException(nameof(getUid));
        }

        private DocumentReference DocForUid(string uid) =>
            _db.Collection(RootCollection).Document(uid);

        /// <summary>
        /// Append using a ScriptableObject. Returns true if sent, false if rejected.
        /// </summary>
        public async Task<bool> AppendEntryAsync(LocationEventSO asset)
        {
            if (asset == null)
            {
                Debug.LogWarning("[LocationData] No asset provided. Skipping send.");
                return false;
            }

            var name = asset.LocationName;
            if (string.IsNullOrWhiteSpace(name))
            {
                Debug.LogWarning("[LocationData] LocationName is missing. Entry rejected.");
                return false;
            }

            // Timestamp captured at send time (UTC)
            var nowUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var ts = Timestamp.FromDateTime(nowUtc);

            try
            {
                await AppendEntryAsync(name, ts);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LocationData] Failed to append entry: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Append using raw values. Throws for invalid input.
        /// </summary>
        private async Task AppendEntryAsync(string locationName, Timestamp time)
        {
            if (string.IsNullOrWhiteSpace(locationName))
                throw new ArgumentException("locationName is required.", nameof(locationName));

            var uid = _getUid();
            var doc = DocForUid(uid);

            var entry = new Dictionary<string, object>
            {
                { "locationName", locationName.Trim() },
                { "time", time }
            };

            await doc.SetAsync(
                new Dictionary<string, object>
                {
                    { EntriesField, FieldValue.ArrayUnion(entry) }
                },
                SetOptions.MergeAll
            );
        }

        /// <summary>
        /// Convenience helper that captures send time. Throws for invalid input.
        /// </summary>
        public Task AppendEntryNowAsync(string locationName)
        {
            var nowUtc = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            return AppendEntryAsync(locationName, Timestamp.FromDateTime(nowUtc));
        }

        /// <summary>
        /// Read back the entries array as a list of dictionaries.
        /// </summary>
        public async Task<IReadOnlyList<Dictionary<string, object>>> GetEntriesAsync(string uid = null)
        {
            uid ??= _getUid();
            var snap = await DocForUid(uid).GetSnapshotAsync();
            if (!snap.Exists) return Array.Empty<Dictionary<string, object>>();

            if (!snap.TryGetValue(EntriesField, out IList<object> raw))
                return Array.Empty<Dictionary<string, object>>();

            var list = new List<Dictionary<string, object>>(raw.Count);
            foreach (var item in raw)
            {
                if (item is Dictionary<string, object> map)
                    list.Add(map);
            }
            return list;
        }
    }
}
