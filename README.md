# RouletteRecorder

FF14 日随记录器，支持导随。

卫月版本见[RouletteRecorder.Dalamud](https://github.com/StarHeartHunt/RouletteRecorder.Dalamud)，适合不想更新 ACT 插件的懒人使用。

[NGA 原帖](https://nga.178.com/read.php?tid=34277964)

## 下载方式

1. 前往[本仓库 Releases](https://github.com/StarHeartHunt/RouletteRecorder/releases)下载

2. [NGA 帖子主楼](https://nga.178.com/read.php?tid=34277964)更新

3. 前往[本仓库 Actions 构建](https://github.com/StarHeartHunt/RouletteRecorder/actions/workflows/build.yml)，点击上方最新 commit 的构建记录，下拉找到 Artifacts 点击 Bundle 下载

## 使用方法

1. 解压压缩包，在 ACT 插件管理选择 RouletteRecorder.dll，添加启用插件

2. 配置订阅任务类型，在对应类型的随机任务结束时数据会写入到插件同目录下的数据.csv 文件中

## 注意事项

在插件工作前，如果使用 excel 等软件打开了数据文件，需要先进行关闭，否则插件无法锁定文件保证随机任务记录写入。

在每次版本更新后插件也需要进行更新，可前往 [GitHub](https://github.com/StarHeartHunt/RouletteRecorder) 获取更新或者发 PR 帮助我适配版本。
