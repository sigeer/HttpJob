## 思想



### 爬虫核心功能

`SpiderWorker`

1. 发起任务
2. 发起子任务

#### 文本替换

设置动态替换域

### 存储

#### 配置存储

#### 结果存储

### 爬虫管理

1. 当前正在执行的任务
2. 已完成的任务
3. 已取消的任务
4. 发起新任务
5. 取消正在执行的任务
6. 输出日志

### 使用方法

```C#
// 读取<div class="text">的InnerHtml
$XHtml(//div[@class="text"])
// 读取InnerHtml，最终可以保存html或者text。读取InnerText，无论设置什么类型，都只能保存text

/// 常用提取

// <div class="page"><ul><li>最后一个li
$XHtml(//div[@class="page"]/ul/li[last()])
```
