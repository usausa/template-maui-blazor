namespace Template.MobileApp.Models.Sample;

public static class DataSample
{
    public static IReadOnlyList<WorkEntity> LoadWorks() =>
    [
        new() { Id = 1, Name = "Sample-1" },
        new() { Id = 2, Name = "Sample-2" },
        new() { Id = 3, Name = "Sample-3" },
        new() { Id = 4, Name = "Sample-4" }
    ];

    public static IReadOnlyList<StoreEntity> LoadStores() =>
    [
        new() { Id = 1, Name = "らーめん 麺屋みほん", Category = StoreCategory.Restaurant, Area = "渋谷", PointRate = 5 },
        new() { Id = 2, Name = "カフェ ことり", Category = StoreCategory.Restaurant, Area = "表参道", PointRate = 3 },
        new() { Id = 3, Name = "定食 さくら食堂", Category = StoreCategory.Restaurant, Area = "新宿", PointRate = 2 },
        new() { Id = 4, Name = "カレー ほしぞら", Category = StoreCategory.Restaurant, Area = "池袋", PointRate = 3 },
        new() { Id = 5, Name = "ミホンマート 駅前店", Category = StoreCategory.Convenience, Area = "新宿", PointRate = 1 },
        new() { Id = 6, Name = "ミホンマート 公園通り店", Category = StoreCategory.Convenience, Area = "渋谷", PointRate = 1 },
        new() { Id = 7, Name = "ことぶき薬局", Category = StoreCategory.Drugstore, Area = "池袋", PointRate = 3 },
        new() { Id = 8, Name = "みどりドラッグ", Category = StoreCategory.Drugstore, Area = "品川", PointRate = 2 },
        new() { Id = 9, Name = "スーパー ひまわり", Category = StoreCategory.Supermarket, Area = "中野", PointRate = 2 },
        new() { Id = 10, Name = "やおや本舗", Category = StoreCategory.Supermarket, Area = "吉祥寺", PointRate = 1 },
        new() { Id = 11, Name = "本の森 書店", Category = StoreCategory.Other, Area = "神保町", PointRate = 5 },
        new() { Id = 12, Name = "クリーニング そよかぜ", Category = StoreCategory.Other, Area = "目黒", PointRate = 1 }
    ];

    public static IReadOnlyList<NoticeEntity> LoadNotices(DateTime now) =>
    [
        new() { Id = 1, Category = NoticeCategory.Campaign, Title = "今月のポイント還元キャンペーン", Body = "対象のお店でのお支払いで、最大 10% のポイントが戻ります。期間は今月の末までです。", PublishedAt = now.AddHours(-3), IsRead = false },
        new() { Id = 2, Category = NoticeCategory.Payment, Title = "らーめん 麺屋みほんで 1,200 円を支払いました", Body = "お支払いが完了しました。ポイントは翌日に付きます。", PublishedAt = now.AddDays(-1), IsRead = false },
        new() { Id = 3, Category = NoticeCategory.Payment, Title = "5,000 円をチャージしました", Body = "チャージが完了しました。残高はアカウントの画面で確かめられます。", PublishedAt = now.AddDays(-2), IsRead = true },
        new() { Id = 4, Category = NoticeCategory.Information, Title = "メンテナンスのお知らせ", Body = "毎週日曜日の 2:00〜4:00 は、サービスを止めて作業をします。この間はお支払いとチャージができません。", PublishedAt = now.AddDays(-3), IsRead = true },
        new() { Id = 5, Category = NoticeCategory.Campaign, Title = "カフェ ことりで使えるようになりました", Body = "表参道のカフェ ことりで、お支払いに使えるようになりました。3% のポイントが戻ります。", PublishedAt = now.AddDays(-5), IsRead = true },
        new() { Id = 6, Category = NoticeCategory.Information, Title = "利用規約を変えます", Body = "来月の 1 日から、新しい利用規約になります。内容はヘルプの画面で確かめられます。", PublishedAt = now.AddDays(-8), IsRead = true }
    ];

    public static IReadOnlyList<HistoryEntity> LoadHistories(DateTime now) =>
    [
        new() { Id = 1, Kind = HistoryKind.Charge, Title = "チャージ", Amount = 10000, OccurredAt = now.AddDays(-7) },
        new() { Id = 2, Kind = HistoryKind.Payment, Title = "スーパー ひまわり", Amount = 2480, OccurredAt = now.AddDays(-6) },
        new() { Id = 3, Kind = HistoryKind.Point, Title = "ポイントの付与", Amount = 25, OccurredAt = now.AddDays(-5) },
        new() { Id = 4, Kind = HistoryKind.Payment, Title = "カフェ ことり", Amount = 650, OccurredAt = now.AddDays(-4) },
        new() { Id = 5, Kind = HistoryKind.Payment, Title = "ことぶき薬局", Amount = 1320, OccurredAt = now.AddDays(-3) },
        new() { Id = 6, Kind = HistoryKind.Charge, Title = "チャージ", Amount = 5000, OccurredAt = now.AddDays(-2) },
        new() { Id = 7, Kind = HistoryKind.Payment, Title = "らーめん 麺屋みほん", Amount = 1200, OccurredAt = now.AddDays(-1) },
        new() { Id = 8, Kind = HistoryKind.Point, Title = "キャンペーンのポイント", Amount = 120, OccurredAt = now.AddHours(-20) },
        new() { Id = 9, Kind = HistoryKind.Payment, Title = "ミホンマート 駅前店", Amount = 398, OccurredAt = now.AddHours(-2) }
    ];
}
