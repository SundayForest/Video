using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoInfrastructure
{
    public class CacheHelp
    {
        private readonly ConnectionMultiplexer redis;
        IDatabase db;
        private CacheHelp() {
            redis = ConnectionMultiplexer.Connect("localhost");
            db = redis.GetDatabase();
        }
        //传入json
        public async Task SetStringAsync(string key, string mes) {
            await db.StringSetAsync(key, mes);
        }
        //传入json with time
        public async Task SetStringWithTimeAsync(string key, string mes,int time)
        {
            await db.StringSetAsync(key, mes,TimeSpan.FromSeconds(time));
        }
        //传出json
        public async Task<string?> GetStringAsync(string key) {
            return await db.StringGetAsync(key);
        }
        //传出json with reset ttl
        public async Task<string?> GetStringWithTimeAsync(string key,int time)
        {
            return await db.StringGetSetExpiryAsync(key,TimeSpan.FromSeconds(time));
        }

        //增加数量
        public async Task AddAsync(string key) { 
           await db.StringIncrementAsync(key);
        }
        //增加数量 + ttl
        public async Task AddWithTimeAsync(string key,int time)
        {
            await db.StringIncrementAsync(key);
            await db.StringGetSetExpiryAsync(key,TimeSpan.FromSeconds(time));
        }

        //取出数量（并重置为0）
        public async Task<int> GetNumAsync(string key) {
            string? num = await db.StringSetAndGetAsync(key, 0);
            if (num.IsNullOrEmpty()) 
            {
                return 0;
            }
            bool b = int.TryParse(num,out int nu);
            if(b)
            {
                return nu;
            }
            return 0;
        }

        //获得sorted set 的数量
        public async Task<long> GetSortedLengthAsync(string key) { 
            return await db.SortedSetLengthAsync(key);
        }

        //往 sorted set 加入数据
        public async Task AddSortedSetAsync(string key, string[] mess, double[] scores) {
            SortedSetEntry[] sortedSets = new SortedSetEntry[mess.Length];
            for(int i = 0; i < sortedSets.Length; i++)
            {
                sortedSets[i] = new SortedSetEntry(mess[i], scores[i]);
            }
            await db.SortedSetAddAsync(key,sortedSets);
        }

        //获得soeted set 数据
        public async Task<(List<string>,List<double>)> GetSortedAsync(string key) {
            var enumerator = db.SortedSetScanAsync(key).GetAsyncEnumerator();
            List<double> scores = new List<double>();
            List<string> mess = new List<string>();
            while (await enumerator.MoveNextAsync())
            {
                mess.Add(enumerator.Current.Element!);
                scores.Add(enumerator.Current.Score);
            }
            return (mess,scores);
        }
    }
}
