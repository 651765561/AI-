using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AiWin_API.Controllers
{
    //启用跨域
    [EnableCors("AllowSameDomain")]
    [Route("api/[controller]")]
    public class NmsController : Controller
    {
       // private readonly ILog _log = LogManager.GetLogger(Program.Repository.Name, typeof(NmsController));

        // GET api/values
        [HttpGet]
        public string Get(Para para)
        {
            //var ipAddress = HttpContext.GetUserIp();
            //var browser = HttpContext.GetUserBrowser();
            JsonMessage msg = new JsonMessage();
            if (!string.IsNullOrEmpty(para.CrsCode))
            {

                if (!string.IsNullOrEmpty(para.CameraOne)) //相机一开||关
                {
                    msg = Ctl.Instance.CameraOne(para.CrsCode, para.CameraOne);

                }
                if (!string.IsNullOrEmpty(para.CameraTwo))//相机二开||关
                {
                    msg = Ctl.Instance.CameraTwo(para.CrsCode, para.CameraTwo);

                }
                if (!string.IsNullOrEmpty(para.CameraThree))//相机三开||关
                {
                    msg = Ctl.Instance.CameraThree(para.CrsCode, para.CameraThree);
                }
                if (!string.IsNullOrEmpty(para.Fot))//光端机重启
                {
                    msg = Ctl.Instance.Fot(para.CrsCode, para.Fot);

                }
                if (!string.IsNullOrEmpty(para.Router))//路由器重启
                {
                    msg = Ctl.Instance.Router(para.CrsCode, para.Router);

                }
                if (!string.IsNullOrEmpty(para.Fan))//风扇开||关
                {
                    msg = Ctl.Instance.Fan(para.CrsCode, para.Fan);

                }
                if (!string.IsNullOrEmpty(para.Acb))//空开开||关
                {
                    msg = Ctl.Instance.Acb(para.CrsCode, para.Acb);

                }
                if (!string.IsNullOrEmpty(para.CloseAlarm))//报警信息开||关
                {
                    msg = Ctl.Instance.CloseAlarm(para.CrsCode, para.CloseAlarm);

                }
                if (!string.IsNullOrEmpty(para.CoolingTemp))//设置冷却温度
                {
                    msg = Ctl.Instance.CoolingTemp(para.CrsCode, para.CoolingTemp);

                }
                if (!string.IsNullOrEmpty(para.AlarmTemp))//设置报警温度
                {
                    msg = Ctl.Instance.AlarmTemp(para.CrsCode, para.AlarmTemp);

                }
                if (!string.IsNullOrEmpty(para.SwitchOnSum))//设置空开合闸次数
                {
                    msg = Ctl.Instance.SwitchOnSum(para.CrsCode, para.SwitchOnSum);

                }
                if (!string.IsNullOrEmpty(para.SwitchInt))//设置空开合闸间隔
                {
                    msg = Ctl.Instance.SwitchInt(para.CrsCode, para.SwitchInt);

                }
                if (!string.IsNullOrEmpty(para.AliveTime))//设置通信链接时间
                {
                    msg = Ctl.Instance.AliveTime(para.CrsCode, para.AliveTime);

                }
                if (!string.IsNullOrEmpty(para.ThresholdVoltage))//设置电池合闸门限电压
                {
                    msg = Ctl.Instance.ThresholdVoltage(para.CrsCode, para.ThresholdVoltage);

                }
            }
            else
            {
                msg = new JsonMessage() { Title = "CrsCode参数不能为空", Message = "请检查参数是否正确", Success = false };

            }
            return msg.ToJson();

        }
    }
    public class Para
    {
        public string CrsCode { get; set; }
        public string CameraOne { get; set; }
        public string CameraTwo { get; set; }
        public string CameraThree { get; set; }
        public string Fot { get; set; }
        public string Router { get; set; }
        public string Fan { get; set; }
        public string Acb { get; set; }
        public string CoolingTemp { get; set; }
        public string AlarmTemp { get; set; }
        public string SwitchOnSum { get; set; }
        public string CloseAlarm { get; set; }
        public string SwitchInt { get; set; }
        public string AliveTime { get; set; }
        public string ThresholdVoltage { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
    }
}
