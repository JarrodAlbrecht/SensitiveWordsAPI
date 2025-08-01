using Dapper;
using System.Data;

namespace SensitiveWordsAPI.Repositories
{
    public class SensitiveWordsRepository : ISensitiveWordsRepository
    {
        private IDbConnection _dbConnection;
        private const string GetAllSensitiveWordsProc = "pr_GetAllSensitiveWords";
        private const string GetSensitiveWordByIdProc = "pr_GetSensitiveWordById";
        private const string UpsertSensitiveWordProc = "pr_UpsertSensitiveWord";
        private const string DeleteSensitiveWordProc = "pr_DeleteSensitiveWord";
        private int DbResponse;

        public SensitiveWordsRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<string>> GetAllSensitiveWords()
        {
            try
            {
                return await _dbConnection.QueryAsync<string>(
                    GetAllSensitiveWordsProc,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<string>();
            }
        }

        public async Task<string> GetSensitiveWordById(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            try
            {
                return await _dbConnection.QueryFirstAsync<string>(
                GetSensitiveWordByIdProc,
                parameters,
                commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<string> UpsertSensitiveWord(string word)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SensitiveWord", word.ToUpper());
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                await _dbConnection.ExecuteAsync(
                                UpsertSensitiveWordProc,
                                parameters,
                                commandType: CommandType.StoredProcedure
                                );

                if (DbResponse <= 0)
                    return word.ToUpper();
                else
                    return string.Empty;
                
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<string> DeleteSensitiveWord(string word)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SensitiveWord", word.ToUpper());
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                await _dbConnection.ExecuteAsync(
                             DeleteSensitiveWordProc,
                             parameters,
                             commandType: CommandType.StoredProcedure
                             );

                if (DbResponse <= 0)
                    return word.ToUpper();
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
