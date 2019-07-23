namespace ay.Controls
{

    public enum SplitViewDisplayMode
    {
        /// <summary>
        /// The pane covers the content when it's open and does not take up space in the
        /// control layout. The pane closes when the user taps outside of it.
        /// </summary>
        Overlay = 0,
        /// <summary>
        /// The pane is shown side-by-side with the content and takes up space in the control
        /// layout. The pane does not close when the user taps outside of it.
        /// </summary>
        Inline = 1,
        /// <summary>
        /// The amount of the pane defined by the CompactPaneLength property is shown side-by-side
        /// with the content and takes up space in the control layout. The remaining part
        /// of the pane covers the content when it's open and does not take up space in the
        /// control layout. The pane closes when the user taps outside of it.
        /// </summary>
        CompactOverlay = 2,
        /// <summary>
        /// The amount of the pane defined by the CompactPaneLength property is shown side-by-side
        /// with the content and takes up space in the control layout. The remaining part
        /// of the pane pushes the content to the side when it's open and takes up space
        /// in the control layout. The pane does not close when the user taps outside of
        /// it.
        /// </summary>
        CompactInline = 3
    }


    public enum SplitViewPanePlacement
    {
        /// <summary>
        /// The pane is shown to the left of the SplitView content.
        /// </summary>
        Left = 0,
        /// <summary>
        /// The pane is shown to the right of the SplitView content.
        /// </summary>
        Right = 1,
        //www.ayjs.net 六安杨洋（AY）拓展
        //2016-6-24 10:55:53
        Top = 2,
        //www.ayjs.net 六安杨洋（AY）拓展
        //2016-6-24 10:55:53
        Bottom = 3
    }






}
