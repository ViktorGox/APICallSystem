namespace APICallSystem.APIRequestBuilder.Query
{
    public enum RequestCompareSetting
    {
        None,

        Equals, // =
        NotEquals, // !=
        EqualsCS, // ILIKE

        IncludesAny, // ILIKE '%a%'
        NotIncludesAny, // NOT ILIKE '%a%'
        IncludesAnyCS, // "LIKE '%a%'
        NotIncludesAnyCS, // NOT LIKE '%a%'

        IncludesAnyStart, // ILIKE 'a%'
        NotIncludesAnyStart, // NOT ILIKE 'a%'
        IncludesAnyStartCS, // LIKE 'a%'
        NotIncludesAnyStartCS, // NOT LIKE 'a%'

        IncludesAnyEnd, // ILIKE '%a'
        NotIncludesAnyEnd, // NOT ILIKE '%a'
        IncludesAnyEndCS, // LIKE '%a'
        NotIncludesAnyEndCS, // NOT LIKE '%a'

        MoreThan, // >
        LessThan, // <
        MoreThanOrEqualsTo, // >=
        LessThanOrEqualsTo, // <=

        IsNull, // IS NULL
        Between, // BETWEEN x AND y
        NotBetween, // NOT BETWEEN x AND y
    }
}
