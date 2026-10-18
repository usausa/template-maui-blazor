SELECT
    COALESCE(SUM(CASE Kind WHEN /*@ charge */1 THEN Amount WHEN /*@ payment */0 THEN -Amount ELSE 0 END), 0) AS Balance,
    COALESCE(SUM(CASE Kind WHEN /*@ point */2 THEN Amount ELSE 0 END), 0) AS Point
FROM
    History
