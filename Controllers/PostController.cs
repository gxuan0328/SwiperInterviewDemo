using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CommunityCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace CommunityCenter.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private SqlFunction sqlFunction;
        private int userId;

        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlFunction = new SqlFunction(_configuration);
            //因目前未實作身分識別機制，暫以此模擬從Middleware解讀出來的使用者資訊
            userId = 1;
        }

        /// <summary>
        /// 新增貼文PUT:post
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseFormat<string> PostPost([FromBody] PostPostDto request)
        {
            ResponseFormat<string> result = new ResponseFormat<string>();

            try
            {
                // 透過資料驗證模型，確認Request該帶的參數是否都有
                if (!ModelState.IsValid)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                // 確認欲使用的PrivacyType已定義
                if (!IsValidPrivacyType(request.PrivacyType_Id))
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                string sql = @"
                INSERT INTO `CommunityCenter`.`Post`
                    (`Content`
                    ,`PrivacyType_Id`
                    ,`User_Id`
                    ,`Admin_Id`)
                VALUES
                    (@Content
                    ,@PrivacyType_Id
                    ,@User_Id
                    ,@Admin_Id)";

                MySqlParameter[] parameters = new[]
                {
                    new MySqlParameter("@Content", MySqlDbType.LongText) { Value = request.Content },
                    new MySqlParameter("@PrivacyType_Id", MySqlDbType.Int32) { Value = request.PrivacyType_Id },
                    new MySqlParameter("@User_Id", MySqlDbType.Int32) { Value = userId },
                    new MySqlParameter("@Admin_Id", MySqlDbType.Int32) { Value = userId }, // Admin_id欄位，用以紀錄對於該筆資料的最後異動者(情境:在系統存在超級管理員時)
                };

                int intAffectRow = sqlFunction.ExecuteSql(sql, parameters);
            }
            catch (Exception e)
            {
                result.StatusCode = Error.Code.DATABASE_ERROR;
                result.Message = e.Message;
                return result;
            }

            result.StatusCode = Error.Code.SUCCESS;
            result.Message = Error.Message[Error.Code.SUCCESS];
            return result;
        }

        /// <summary>
        /// 更新貼文PUT:post/:id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ResponseFormat<string> PutPost(int id, [FromBody] PutPostDto request)
        {
            ResponseFormat<string> result = new ResponseFormat<string>();

            try
            {
                // 透過資料驗證模型，確認Request該帶的參數是否都有
                if (!ModelState.IsValid)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                // 檢查參數一致性，確認欲更動目標的id
                if (id != request.Id)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                // 確認是否為該筆異動資料的擁有者
                if (!IsValidPost(id, userId))
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                // 確認欲使用的PrivacyType已定義
                if (!IsValidPrivacyType(request.PrivacyType_Id))
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                string sql = @"
                UPDATE `CommunityCenter`.`Post`
                SET `Content` = @Content
                    ,`PrivacyType_Id` = @PrivacyType_Id
                    ,`Admin_Id` = @Admin_Id
                    ,`UpdateTime` = UTC_TIMESTAMP()
                WHERE `Id` = @Id";

                MySqlParameter[] parameters = new[]
                {
                    new MySqlParameter("@Id", MySqlDbType.Int32) { Value = request.Id },
                    new MySqlParameter("@Admin_Id", MySqlDbType.Int32) { Value = userId }, // Admin_id欄位，用以紀錄對於該筆資料的最後異動者(情境:在系統存在超級管理員時)
                    new MySqlParameter("@PrivacyType_Id", MySqlDbType.Int32) { Value = request.PrivacyType_Id },
                    new MySqlParameter("@Content", MySqlDbType.LongText) { Value = request.Content },
                };

                int intAffectRow = sqlFunction.ExecuteSql(sql, parameters);
            }
            catch (Exception e)
            {
                result.StatusCode = Error.Code.DATABASE_ERROR;
                result.Message = e.Message;
                return result;
            }

            result.StatusCode = Error.Code.SUCCESS;
            result.Message = Error.Message[Error.Code.SUCCESS];
            return result;
        }

        /// <summary>
        /// 刪除貼文DELETE:post/:id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseFormat<string> DeletePost(int id, [FromBody] DeletePostDto request)
        {
            ResponseFormat<string> result = new ResponseFormat<string>();

            try
            {
                // 透過資料驗證模型，確認Request該帶的參數是否都有
                if (!ModelState.IsValid)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                // 檢查參數一致性，確認欲更動目標的id
                if (id != request.Id)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                // 確認是否為該筆異動資料的擁有者
                if (!IsValidPost(id, userId))
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                string sql = @"
                UPDATE `CommunityCenter`.`Post`
                SET `Admin_Id` = @Admin_Id
                    ,`UpdateTime` = UTC_TIMESTAMP()
                    ,`Archive` = 1
                WHERE `Id` = @Id";

                MySqlParameter[] parameters = new[]
                {
                    new MySqlParameter("@Id", MySqlDbType.Int32) { Value = request.Id },
                    new MySqlParameter("@Admin_Id", MySqlDbType.Int32) { Value = userId }, // Admin_id欄位，用以紀錄對於該筆資料的最後異動者(情境:在系統存在超級管理員時)
                };

                int intAffectRow = sqlFunction.ExecuteSql(sql, parameters);
            }
            catch (Exception e)
            {
                result.StatusCode = Error.Code.DATABASE_ERROR;
                result.Message = e.Message;
                return result;
            }

            result.StatusCode = Error.Code.SUCCESS;
            result.Message = Error.Message[Error.Code.SUCCESS];
            return result;
        }

        /// <summary>
        /// 檢查 Post 是否合理
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool IsValidPost(int postId, int userId)
        {
            bool boolFlag = false;

            string sql = @"
            SELECT `Id`
            FROM `CommunityCenter`.`Post`
            WHERE `Id` = @Id
            AND `User_Id` = @User_Id
            AND `Archive` = 0";

            MySqlParameter[] parameters = new[]
            {
                new MySqlParameter("@Id", MySqlDbType.Int32) { Value = postId },
                new MySqlParameter("@User_Id", MySqlDbType.Int32) { Value = userId },
            };

            DataTable dtData = sqlFunction.GetData(sql, parameters);

            if (dtData.Rows.Count == 1)
            {
                boolFlag = true;
            }

            return boolFlag;
        }

        /// <summary>
        /// 檢查 PrivacyType 是否合理
        /// </summary>
        /// <param name="privacyTypeId"></param>
        /// <returns></returns>
        private bool IsValidPrivacyType(int privacyTypeId)
        {
            bool boolFlag = false;

            string sql = @"
            SELECT `Id`
            FROM `CommunityCenter`.`PrivacyType`
            WHERE `Id` = @Id
            AND `Archive` = 0";

            MySqlParameter[] parameters = new[]
            {
                new MySqlParameter("@Id", MySqlDbType.Int32) { Value = privacyTypeId },
            };

            DataTable dtData = sqlFunction.GetData(sql, parameters);

            if (dtData.Rows.Count == 1)
            {
                boolFlag = true;
            }

            return boolFlag;
        }
    }
}
