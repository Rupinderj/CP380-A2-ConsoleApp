using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var a = BreadmakerDb.Breadmakers
                // TODO: add LINQ logic ...
                //       ...
                .Select(item => new
            {
                detail = item.title,
                reviews = item.Reviews.Count(),
                avg = (Double)BreadmakerDb.Reviews
                    .Where(i =>i.BreadmakerId == item.BreadmakerId)
                    .Select(i => i.stars).Sum() / item.Reviews.Count(),
            })

                .ToList();

                var BMList = a
                .Select(item => new
                {
                    detail = item.detail,
                    reviews = item.reviews,
                    avg = item.avg,
                    adjust = ratingAdjustmentService.Adjust(item.avg, item.reviews)
                })
                .OrderByDescending(i => i.adjust)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var item = BMList[j];
                // TODO: add output
                // Console.WriteLine( ... );
                Console.WriteLine($"[{j + 1}]  {item.reviews,7}  {Math.Round(item.avg, 2),-7}  {Math.Round(item.adjust, 2),-6}   {item.detail}");
            }
        }
    }
}
