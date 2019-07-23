阅读工具 http://tool.oschina.net/markdown/
# WPF Client MVC 使用规范

## 面向对象
* mvc架构模式，通过AYUI8插件创建 
* 非mvc架构，通过vs创建的默认的wpf项目

 ## 大致功能描述
1. xaml快速绑定一个事件，可以传递事件参数到一个Controller下的Action中
2. xaml对一个控件，快速绑定多个事件到一个Controller下的N个Action中
3. 绑定执行时候，可以通过过滤器拦截（执行前后，执行是否能继续，初始化绑定执行，绑定时数据共享，异常全局过滤）
4. 拦截域：controller级别，Action级别，多个同类或者不同类的拦截器，可以设置执行顺序和执行级别
5. xaml.cs后台代码，可以快速使用mvc的功能，直接调用Controller中的Action，也可以使用过滤器的方式执行Controller中的Action，也可以模拟传递参数
6. 通过插件创建的wpf项目，可以快速设置，是否单例wpf的程序，一次只能运行一个exe，或者多个exe。也可以快速强类型设置启动页面，无需繁杂的路径
7. Ay插件支持的代码片段
    1. 在 xaml上 输入aymvc然后tab+tab，快速创建mvc的xaml级别的路由代码，也可以在xaml元素上使用Mvc.Event,Parameter,Action
    2. 在Controller级别里面，输入ayaction  然后tab+tab  快速创建  action代码段
	3. 在Model中输入  ayprop  然后tab+tab 快速创建 具有通知的属性
	4. 输入ayconst 然后tab+tab 创建常量
    5. 输入propr创建只读的依赖属性
	6. 输入aym 快速创建方法
	7. 输入aysen 快速创建发送消息 片段代码
    8. 输入ayreg 注册消息
    9. 输入aypropd创建一个依赖属性
    10. 输入aypropdpcallback创建一个具有属性变化回调的依赖属性
    11.  输入aypropsingle创建一个单例的属性
    12.  输入propa创建一个附加属性（vs自带的）
    13.  输入ayregevent创建一个事件和其调用
    14.  输入ayregeventwithargs创建一个自定义事件的参数的类

8. Ay插件支持的页面定位跳转
    1. 在xaml代码中右键，可以前往对应的   Controller和Model层，按下F7快速到xaml.cs文件
    2. 在Controller中右键，可以前往对应的View和Model层，还有xaml.cs层
    3. 在Model中右键，可以前往对应的View和Controller层，还有xaml.cs层
    4. 在xaml.cs中右键，可以前往Model和View和Controller层
    5. WPF项目中右键，可以复制packuri路径，快速前往 消息key的文件 和 Session的Key文件

9. Ay插件支持的项目模板
    1. AYUI8项目，默认结构的标准的AyMvc的项目

10. Ay插件支持的项模板
    1. 验证器，3种级别的，创建后，默认代码上面注释有用法
    2. AyWindow 创建一个MVC结构的窗体
    3. UserControl创建一个MVC结构的用户控件
    4. Page创建一个MVC结构的Page控件
    5. AyConverter创建一个Ay标准的值转换器

 ### mvc架构模式


