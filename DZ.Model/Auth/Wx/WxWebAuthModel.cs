namespace DZ.Model.Auth.Wx
{
    using Framework.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 微信网页授权
    /// </summary>
    public class WxWebAuthModel : BaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Code不能为空")]
        public string code { get; set; }
    }
}
