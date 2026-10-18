SELECT
    *
FROM
    Store
WHERE
    1 = 1
/*% if (keyword != null) { */
    AND (Name LIKE /*@ keyword */'' ESCAPE '\' OR Area LIKE /*@ keyword */'' ESCAPE '\')
/*% } */
/*% if (category != null) { */
    AND Category = /*@ category */0
/*% } */
ORDER BY
    Id
