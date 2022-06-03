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
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private SqlFunction sqlFunction;
        private int userId;

        public CommentController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlFunction = new SqlFunction(_configuration);
            //因目前未實作身分識別機制，暫以此模擬從Middleware解讀出來的使用者資訊
            userId = 1;
        }

        /// <summary>
        /// 新增留言POST:comment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseFormat<string> PostComment([FromBody] PostCommentDto request)
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

                // 分為文章留言、子留言，兩種情境進行處理
                if (request.Parent_Comment_Id == 0)
                {
                    // 若為文章留言，確認欲進行留言的文章存在
                    if (!IsValidPost(request.Post_Id))
                    {
                        result.StatusCode = Error.Code.BAD_REQUEST;
                        result.Message = Error.Message[Error.Code.BAD_REQUEST];
                        return result;
                    }
                }
                else
                {
                    // 若為子留言，確認欲進行回覆的留言正確存在
                    if (!IsValidParentComment(request.Post_Id, request.Parent_Comment_Id))
                    {
                        result.StatusCode = Error.Code.BAD_REQUEST;
                        result.Message = Error.Message[Error.Code.BAD_REQUEST];
                        return result;
                    }
                }

                string sql = @"
                INSERT INTO `CommunityCenter`.`Comment`
                    (`Content`
                    ,`User_Id`
                    ,`Admin_Id`)
                VALUES
                    (@Content
                    ,@User_Id
                    ,@Admin_Id);

                SET @Comment_Id = LAST_INSERT_ID();

                INSERT INTO `CommunityCenter`.`Post_Comment_Relation`
                    (`Post_Id`
                    ,`Parent_Comment_Id`
                    ,`Comment_Id`
                    ,`Admin_Id`)
                VALUES
                    (@Post_Id
                    ,@Parent_Comment_Id
                    ,@Comment_Id
                    ,@Admin_Id);";

                MySqlParameter[] parameters = new[]
                {
                    new MySqlParameter("@Content", MySqlDbType.LongText) { Value = request.Content },
                    new MySqlParameter("@User_Id", MySqlDbType.Int32) { Value = userId },
                    new MySqlParameter("@Admin_Id", MySqlDbType.Int32) { Value = userId }, // Admin_id欄位，用以紀錄對於該筆資料的最後異動者(情境:在系統存在超級管理員時)
                    new MySqlParameter("@Post_Id", MySqlDbType.Int32) { Value = request.Post_Id },
                    new MySqlParameter("@Parent_Comment_Id", MySqlDbType.Int32) { Value = request.Parent_Comment_Id },
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
        /// 更新留言PUT:comment/:id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ResponseFormat<string> PutComment(int id, [FromBody] PutCommentDto request)
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
                if (!IsValidComment(id, userId))
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                string sql = @"
                UPDATE `CommunityCenter`.`Comment`
                SET `Content` = @Content
                    ,`Admin_Id` = @Admin_Id
                    ,`UpdateTime` = UTC_TIMESTAMP()
                WHERE `Id` = @Id";

                MySqlParameter[] parameters = new[]
                {
                    new MySqlParameter("@Id", MySqlDbType.Int32) { Value = request.Id },
                    new MySqlParameter("@Admin_Id", MySqlDbType.Int32) { Value = userId }, // Admin_id欄位，用以紀錄對於該筆資料的最後異動者(情境:在系統存在超級管理員時)
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
        /// 刪除留言DELETE:comment/:id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ResponseFormat<string> DeleteComment(int id, [FromBody] DeleteCommentDto request)
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
                if (!IsValidComment(id, userId))
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }

                string sql = @"
                UPDATE `CommunityCenter`.`Comment`
                SET `Admin_Id` = @Admin_Id
                    ,`UpdateTime` = UTC_TIMESTAMP()
                    ,`Archive` = 1
                WHERE `Id` = @Id;

                UPDATE `CommunityCenter`.`Post_Comment_Relation`
                SET `Admin_Id` = @Admin_Id
                    ,`UpdateTime` = UTC_TIMESTAMP()
                    ,`Archive` = 1
                WHERE `Comment_Id` = @Id;";

                MySqlParameter[] parameters = new[]
                {
                    new MySqlParameter("@Id", MySqlDbType.Int32) { Value = request.Id },
                    new MySqlParameter("@Admin_Id", MySqlDbType.Int32) { Value = userId },
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
        /// 檢查 Comment 是否合理
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool IsValidComment(int commentId, int userId)
        {
            bool boolFlag = false;

            string sql = @"
            SELECT `Id`
            FROM `CommunityCenter`.`Comment`
            WHERE `Id` = @Id
            AND `User_Id` = @User_Id
            AND `Archive` = 0";

            MySqlParameter[] parameters = new[]
            {
                new MySqlParameter("@Id", MySqlDbType.Int32) { Value = commentId },
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
        /// 檢查 Parent_Comment 是否合理
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="parentCommentId"></param>
        /// <returns></returns>
        private bool IsValidParentComment(int postId, int parentCommentId)
        {
            bool boolFlag = false;

            string sql = @"
            SELECT `Id`
            FROM `CommunityCenter`.`Post_Comment_Relation`
            WHERE `Post_Id` = @Post_Id
            AND `Comment_Id` = @Comment_Id
            AND `Archive` = 0";

            MySqlParameter[] parameters = new[]
            {
                new MySqlParameter("@Post_Id", MySqlDbType.Int32) { Value = postId },
                new MySqlParameter("@Comment_Id", MySqlDbType.Int32) { Value = parentCommentId },
            };

            DataTable dtData = sqlFunction.GetData(sql, parameters);

            if (dtData.Rows.Count == 1)
            {
                boolFlag = true;
            }

            return boolFlag;
        }

        /// <summary>
        /// 檢查 Post 是否合理
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        private bool IsValidPost(int postId)
        {
            bool boolFlag = false;

            string sql = @"
            SELECT `Id`
            FROM `CommunityCenter`.`Post`
            WHERE `Id` = @Id
            AND `Archive` = 0";

            MySqlParameter[] parameters = new[]
            {
                new MySqlParameter("@Id", MySqlDbType.Int32) { Value = postId },
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
