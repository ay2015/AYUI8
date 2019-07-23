阅读工具 http://tool.oschina.net/markdown/

# Action
操作性质的控件，例如按钮，单选，复选，重复选，thumb

# Feedback
反馈类控件，例如通知，弹窗，气泡，录入反馈

# Input
接收光标输入的焦点，接受选择的焦点

# Layout
容器类，布局类的控件，窗体也属于

# Render
展示类控件，列入图片列表 ，表格

# Select
选择类控件，通过弹窗后选择的

# Primitive
基本控件，解决自带控件的一些基本问题
> ## AyPopup
> > 
解决 跟着窗体移动，窗体最大化还原定位，但是不会跟着 定位控件 的移动而动，只会根据窗体的locationchanged
> > 
解决 置顶其他应用，单击其他区域自动关闭

> ## AyBigPopup
> 
解决最大面积高度问题，popup不允许超过win的三分之二的高度，使用AyBigPopup弹层解决，比如全屏菜单
