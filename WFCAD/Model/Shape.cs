﻿using System;
using System.Drawing;

namespace WFCAD {
    /// <summary>
    /// 図形クラス
    /// </summary>
    public abstract class Shape : IShape {

        #region プロパティ

        /// <summary>
        /// 始点
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// 終点
        /// </summary>
        public Point EndPoint { get; set; }

        /// <summary>
        /// 幅
        /// </summary>
        protected int Width => Math.Abs(this.StartPoint.X - this.EndPoint.X);

        /// <summary>
        /// 高さ
        /// </summary>
        protected int Height => Math.Abs(this.StartPoint.Y - this.EndPoint.Y);

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 描画オプション
        /// </summary>
        public Pen Option { get; set; }

        #endregion プロパティ

        #region メソッド

        /// <summary>
        /// 描画します
        /// </summary>
        public abstract void Draw(Graphics vGraphics);

        /// <summary>
        /// 指定した座標が図形内に存在するか
        /// </summary>
        public abstract bool IsHit(Point vMouseLocation);

        /// <summary>
        /// 複製します
        /// </summary>
        public IShape DeepClone() {
            IShape wShape = this.DeepCloneCore();
            wShape.StartPoint = this.StartPoint;
            wShape.EndPoint = this.EndPoint;
            wShape.IsSelected = this.IsSelected;
            return wShape;
        }

        /// <summary>
        /// 派生クラスごとのインスタンスを返します
        /// </summary>
        protected abstract IShape DeepCloneCore();

        #endregion メソッド
    }
}
