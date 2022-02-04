using ExtensionsLibrary.Extensions;
using Microsoft.EntityFrameworkCore;
using MusicPlayerLibrary.Interfaces;
using MusicPlayerLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayerLibrary.Helpers.Extensions
{
    public static class DBSetExtensions
    {
        public static void AddOrUpdate<TEntity>(this DbSet<TEntity> collection, TEntity item) where TEntity : class, IEntity
        {
            if (item is not BaseMusicModel musicModel || musicModel.IsSaveEnabled)
            {
                if (!collection.Exists(item)) collection.Add(item);
                else collection.Update(item);
            }
        }

        public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> collection, IEnumerable<TEntity> source) where TEntity : class, IEntity
        {
            foreach (TEntity item in source) collection.AddOrUpdate(item);
        }

        public static void AddOrIgonre<TEntity>(this DbSet<TEntity> collection, TEntity item) where TEntity : class, IEntity
        {
            if (item is not BaseMusicModel musicModel || musicModel.IsSaveEnabled) if (!collection.Exists(item)) collection.Add(item);
        }

        public static void AddOrIgnoreRange<TEntity>(this DbSet<TEntity> collection, IEnumerable<TEntity> source) where TEntity : class, IEntity
        {
            foreach (TEntity item in source) collection.AddOrIgonre(item);
        }

        public static bool Exists<TEntity>(this DbSet<TEntity> collection, TEntity item) where TEntity : class, IEntity
        {
            return !(collection.Find(item.ID) is null) || collection.Contains(item);
        }

        public static void AddRange<TEntity>(this DbSet<TEntity> targetCollection, IEnumerable<TEntity> sourceCollection) where TEntity : class
        {
            sourceCollection.ForEach(I => targetCollection.Add(I));
        }

        public static void AddIfDoesntContain<TEntity>(this DbSet<TEntity> collection, TEntity item) where TEntity : class, IEntity
        {
            if (item != null && !collection.Any(E => E.ID == item.ID)) collection.Add(item);
        }
    }
}