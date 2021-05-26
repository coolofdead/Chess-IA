public class ChessDataProviderFactory
{
    public IDataProvidable GetDataProvidable()
    {
        return new ChessComAPI();
    }
}
