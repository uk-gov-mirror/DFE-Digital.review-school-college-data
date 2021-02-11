namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public enum ValidatorType
    {
        None,
        Regex,
        DateTimeHistorical,
        DateTimeFuture,
        AlphabeticalValues,
        AlphabeticalIncludingSpecialChars,
        Number,
        NumberHigher,
        NumberLower,
        NumberHigherOrEqual,
        NumberLowerOrEqual,
        LAESTABNumber
    }
}