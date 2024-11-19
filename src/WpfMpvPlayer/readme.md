# 测试 mpv 播放视频

官方示例参考：[winform-demo](https://github.com/mpv-player/mpv-examples/blob/master/libmpv/csharp/Form1.cs)

调用函数参考：[mpv.net-Player](https://github.com/mpvnet-player/mpv.net/blob/main/src/MpvNet/Player.cs)

调用库见：[libmpv-2.dll](https://github.com/shinchiro/mpv-winbuild-cmake/releases)
例如我下载的包为: mpv-dev-x86_64-20241103-git-42ff6f9.7z

运行前，请确保工作目录内，有库文件"libmpv-2.dll"。

## 精简用法

- 创建播放器句柄 (create)
- 初始化播放器 (init)
- 关联显示控件句柄 (wid)
- 打开本地视频文件并播放 (loadfile)