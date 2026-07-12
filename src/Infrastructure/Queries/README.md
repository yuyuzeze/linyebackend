# Queries Catalog（画面読取 SQL）

## Repository / Gateway の使い分け（形状ルール。「簡単／複雑」で判断しない）

```text
書込み（Insert/Update/Delete）              → IXxxRepository（EF）
Id で Entity を取得し、直後に更新して Save → IXxxRepository.GetById
それ以外の画面読取（一覧／検索／DTO）       → IQueryGateway + *Queries（QueryKey 必須）
迷ったら                                 → デフォルトで Gateway
```

## Repository ホワイトリスト

汎用 `IRepository<T>` のみ使う（個別 XxxRepository は原則作らない）。

許可：`GetByIdAsync` / `AddAsync` / `UpdateAsync` / `RemoveAsync`

禁止：`GetAll` / `Find(predicate)` / `Query() → IQueryable` / `Search*` / `List*` / 複数テーブル Include 一覧。

## Key ルール

- Catalog の **フィールド名** = QueryKey（例：`HOSHO_Q001`）
- フィールド値 = SQL テキスト
- 呼び出し：`nameof(XxxQueries.HOSHO_Q001)` + `XxxQueries.HOSHO_Q001`
- ドメイン内で連番を増やし、廃止した番号は再利用しない
- 他業務から呼ぶ場合：相手ドメインのフィールドを参照し、SQL をコピーしない

## ディレクトリ

```text
Queries/
  Kyotsu/   ← 共通担当
  Hosho/    ← 保証担当
  Satei/    ← 査定担当
DataAccess/
  IQueryGateway.cs
  DapperQueryGateway.cs
```

## 呼び出し例

```csharp
var rows = await _queries.QueryAsync<DemoItemListRow>(
    nameof(KyotsuQueries.KYOTSU_Q001),  // Key = フィールド名
    KyotsuQueries.KYOTSU_Q001,          // SQL
    cancellationToken: ct);
```

Catalog には SQL のみ置く。**フィールド名が QueryKey**。呼び出し時に `nameof(...)` で Gateway に渡し、ログに残す。
