bat安装方法：
cd /d %~dp0
@echo on
color 2f
mode con: cols=80 lines=25
echo 请按任意键开始安装智能机箱系统服务...
pause
AiWin_New install
AiWin_New start
pause
