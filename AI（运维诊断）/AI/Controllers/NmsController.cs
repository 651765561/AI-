using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
namespace AI.Controllers
{
    //启用跨域
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    public class NmsController : Controller
    {
        private readonly Ctl _ctl;
        public NmsController(Ctl ctl)
        {
            var ele = Di.ServiceProvider.GetRequiredService<Ctl>();
            var t3 = HttpContext.RequestServices.GetService(typeof(Ctl)) as Ctl;
            //  var para = ele.CameraOne();
            _ctl = ctl;
        }
        // private readonly ILog _log = LogManager.GetLogger(Program.Repository.Name, typeof(NmsController));
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
                    msg = _ctl.CameraOne(para.CrsCode, para.CameraOne);

                }
                if (!string.IsNullOrEmpty(para.CameraTwo))//相机二开||关
                {
                    msg = _ctl.CameraTwo(para.CrsCode, para.CameraTwo);

                }
                if (!string.IsNullOrEmpty(para.CameraThree))//相机三开||关
                {
                    msg = _ctl.CameraThree(para.CrsCode, para.CameraThree);
                }
                if (!string.IsNullOrEmpty(para.Fot))//光端机重启
                {
                    msg = _ctl.Fot(para.CrsCode, para.Fot);

                }
                if (!string.IsNullOrEmpty(para.Router))//路由器重启
                {
                    msg = _ctl.Router(para.CrsCode, para.Router);

                }
                if (!string.IsNullOrEmpty(para.Fan))//风扇开||关
                {
                    msg = _ctl.Fan(para.CrsCode, para.Fan);

                }
                if (!string.IsNullOrEmpty(para.Acb))//空开开||关
                {
                    msg = _ctl.Acb(para.CrsCode, para.Acb);

                }
                if (!string.IsNullOrEmpty(para.CloseAlarm))//报警信息开||关
                {
                    msg = _ctl.CloseAlarm(para.CrsCode, para.CloseAlarm);

                }
                if (!string.IsNullOrEmpty(para.CoolingTemp))//设置冷却温度
                {
                    msg = _ctl.CoolingTemp(para.CrsCode, para.CoolingTemp);

                }
                if (!string.IsNullOrEmpty(para.AlarmTemp))//设置报警温度
                {
                    msg = _ctl.AlarmTemp(para.CrsCode, para.AlarmTemp);

                }
                if (!string.IsNullOrEmpty(para.SwitchOnSum))//设置空开合闸次数
                {
                    msg = _ctl.SwitchOnSum(para.CrsCode, para.SwitchOnSum);

                }
                if (!string.IsNullOrEmpty(para.SwitchInt))//设置空开合闸间隔
                {
                    msg = _ctl.SwitchInt(para.CrsCode, para.SwitchInt);

                }
                if (!string.IsNullOrEmpty(para.AliveTime))//设置通信链接时间
                {
                    msg = _ctl.AliveTime(para.CrsCode, para.AliveTime);

                }
                if (!string.IsNullOrEmpty(para.ThresholdVoltage))//设置电池合闸门限电压
                {
                    msg = _ctl.ThresholdVoltage(para.CrsCode, para.ThresholdVoltage);

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
