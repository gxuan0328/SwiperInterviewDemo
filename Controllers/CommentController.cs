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

        public CommentController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlFunction = new SqlFunction(_configuration);
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
                if (!ModelState.IsValid)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }


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
        public ResponseFormat<string> PutComment([FromQuery] int id, [FromBody] PutCommentDto request)
        {
            ResponseFormat<string> result = new ResponseFormat<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }


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
        public ResponseFormat<string> DeleteComment([FromQuery] int id, [FromBody] DeleteCommentDto request)
        {
            ResponseFormat<string> result = new ResponseFormat<string>();

            try
            {
                if (!ModelState.IsValid)
                {
                    result.StatusCode = Error.Code.BAD_REQUEST;
                    result.Message = Error.Message[Error.Code.BAD_REQUEST];
                    return result;
                }


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
    }
}
