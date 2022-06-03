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

        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlFunction = new SqlFunction(_configuration);
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
        public ResponseFormat<string> PutPost([FromQuery] int id, [FromBody] PutPostDto request)
        {
            ResponseFormat<string> result = new ResponseFormat<string>();

            try
            {

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
        public ResponseFormat<string> DeletePost([FromQuery] int id, [FromBody] DeletePostDto request)
        {
            ResponseFormat<string> result = new ResponseFormat<string>();

            try
            {

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
