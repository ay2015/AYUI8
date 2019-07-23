阅读工具 http://tool.oschina.net/markdown/

# 程序集内容共享 

## 设计和需求
 每个程序都应该 有个自己的contents程序集
 共享内容如下
* 图标方案：path,多段彩色path，gif,bmp,png,ico,jpg,fontawesome字体图标
* 字体文件(不推荐使用了，请自己在安装包时候安装,这里不考虑了)
* 资源图片文件，输出到目录->图片文件
* 输出到目录->程序集级别的配置文件
* 多开发者，多程序集之间共享这些内容，例如一个公司多套产品，图标app.ico是同一个，不能多个exe代码下都放一个ico吧，后期遇到换logo了，岂不是很多地方图片都要换。
* 多国语言问题，换言之就是字符串 字典管理，每个开发程序集的人，开发完提供一个dll和一个语言包文件夹提供给你（第三方语言包），然后你把他的语言包放入此程序集方便管理
* AY控件默认是中文。支持多内容库集成
* 按照约定，所有控件库 根目录应该有个All.xaml，这样不用记录复杂样式了
 ```xml  
 <ResourceDictionary Source="pack://application:,,,/程序集名字;component/All.xaml"/>
```
内容库：多语言，图标，字体，包括持久化保存设置，强类型xaml开发提示，后台便捷处理，多内容库合并

## 优势
* 多个 支持AY国际化标准的 程序集 共存，你用你内容包，我用我的，互不干扰，主项目一起使用
* 一个GUI控件 轻松集成多语言 和 字体管理 图标管理 
* 语言包开发完成后，有工具可以快速拓展自己的语言包，也可以第三方帮你拓展

## 文件夹说明
 Content/Lang文件夹（ay国际化规范）
存放多国语言的文件夹，每个程序支持“ay国际化”规范
> lang 一级目录是 客户端下拉框的选项的Text为名字的文件夹，每个文件夹下存放 *.aylang文件。  
命名规则：推荐用 程序集名字.aylang  
所以二级目录下会有N个程序集的多国语言字典文件。  
ay会提供aylang的翻译成资源字典和C#资源key的类 的工具，方便支持wpf UI可视化和资源key的代码中的强类型快捷提示



使用思路:
新建一个Ay内容程序集项目，这里叫ay.contents
你只需要关注的是Content下的文件内容。

内容库设置集成到项目
在App.xaml.cs中写入  
```C#
  protected override void OnStartup(StartupEventArgs e)  
        {
            base.OnStartup(e);
            ContentManager.Instance.ApplySetting();
        }  
```
自己放个控件，集成语言设置功能
```xml
     <ay:AyLangComboBox x:Name="lcboSetting"  Height="38" Width="150" Margin="10,0,0,0"/>
```
后台保存语言设置
```C#
          lcboSetting.SaveSettingAndAppy();
```
集成字体设置，找个控件，绑定行为  
```xml
顶部空间 xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"  
<i:Interaction.Triggers>
                    <i:EventTrigger EventName="Click">
                        <ay:FontFamilyDialogPicker/>
                    </i:EventTrigger>
                </i:Interaction.Triggers>
```


xaml强类型提示，在App.xaml中引用资源
```xml
<Application x:Class="AyWpfProject.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:AyWpfProject"
             xmlns:ay="http://www.ayjs.net/2020"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/ay.contentcore;component/All.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/ay.contents;component/All.xaml"/>
            </ResourceDictionary.MergedDictionaries>
            <ay:DesignDevSupport x:Key="design" ContentDirectory="E:\新建文件夹\AYUI7\AyWpfProject\bin\Debug\Content"/>
        </ResourceDictionary>
    </Application.Resources>
</Application>


```
LangToDevFileConsole会把递归当前目录下所有的aylang文件，然后生成文件，输出在上一级目录  
后台代码获得字符串  
单独



T4修复参考  
<#@ assembly name="System.Core.dll" #>
<#@ assembly name="System.Data.dll" #>
<#@ assembly name="System.Data.DataSetExtensions.dll" #>
<#@ assembly name="System.Xml.dll" #>
<#@ import namespace="System" #>
<#@ import namespace="System.Xml" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Data" #>
<#@ import namespace="System.Data.SqlClient" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ import namespace="System.IO" #>
<#@ output extension=".txt" #>
Hello World! 