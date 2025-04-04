﻿using System;
using System.Collections.Generic;
using LostAngeles.Shared;
using NLog;

namespace LostAngeles.Server.Core
{
    public static class SpawnHelper
    {
        private static readonly Logger Log = LogManager.GetLogger("SPAWNHELPER");
        
        private static List<PlayerPosition> _spawnPositions;
        private static int _lastSpawnIndex = 0;

        public static void InitializePositions(List<PlayerPosition> positions)
        {
            _spawnPositions = positions;
            _spawnPositions.Shuffle();
            
            Log.Info($"Initialized positions[{positions.Count}].");
        }
        
        public static PlayerPosition GetNextSpawnPosition()
        {
            _lastSpawnIndex = (_lastSpawnIndex + 1) % _spawnPositions.Count;
            return _spawnPositions[_lastSpawnIndex];
        }

        private static void Shuffle(this IList<PlayerPosition> positions)
        {
            var rng = new Random();
            var n = positions.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (positions[k], positions[n]) = (positions[n], positions[k]);
            }
        }
    }
}