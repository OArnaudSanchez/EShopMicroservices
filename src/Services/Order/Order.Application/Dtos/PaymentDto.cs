namespace Order.Application.Dtos
{
    public record PaymentDto(
        string CardName,
        string CardNumber,
        string Expiration, //TODO: Maybe we should have separate fields, int Day, int Year and concat those values to send to ValueObject
        string Cvv,
        int PaymentMethod);//TODO: What are the valid payment methods? Maybe we should have a catalog or enum for that
}