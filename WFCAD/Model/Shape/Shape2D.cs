﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WFCAD.Model.Frame;

namespace WFCAD.Model.Shape {
    /// <summary>
    /// 2次元図形クラス
    /// </summary>
    public abstract class Shape2D : Shape, IShape2D {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Shape2D(Color vColor) : base(vColor) { }

        /// <summary>
        /// 外枠
        /// </summary>
        public System.Drawing.Rectangle FrameRectangle => new System.Drawing.Rectangle(this.StartPoint.X, this.StartPoint.Y, this.Width, this.Height);

        /// <summary>
        /// 始点と終点を設定します
        /// </summary>
        public override void SetPoints(Point vStartPoint, Point vEndPoint) {
            // 引数で受け取った始点と終点を対角線とする矩形に対して、
            // 左上の点と右下の点を始点と終点に設定します。
            this.StartPoint = new Point(Math.Min(vStartPoint.X, vEndPoint.X), Math.Min(vStartPoint.Y, vEndPoint.Y));
            this.EndPoint = new Point(Math.Max(vStartPoint.X, vEndPoint.X), Math.Max(vStartPoint.Y, vEndPoint.Y));

            // 枠点の座標
            var wTopLeft = this.StartPoint;
            var wTop = new Point(this.StartPoint.X + (this.EndPoint.X - this.StartPoint.X) / 2, this.StartPoint.Y);
            var wTopRight = new Point(this.EndPoint.X, this.StartPoint.Y);
            var wLeft = new Point(this.StartPoint.X, this.StartPoint.Y + (this.EndPoint.Y - this.StartPoint.Y) / 2);
            var wRight = new Point(this.EndPoint.X, this.StartPoint.Y + (this.EndPoint.Y - this.StartPoint.Y) / 2);
            var wBottomLeft = new Point(this.StartPoint.X, this.EndPoint.Y);
            var wBottom = new Point(this.StartPoint.X + (this.EndPoint.X - this.StartPoint.X) / 2, this.EndPoint.Y);
            var wBottomRight = this.EndPoint;

            // 枠点と基準点の設定
            this.FramePoints = new List<IFramePoint> {
                new FramePoint(wTopLeft, wBottomRight),
                new FramePoint(wTop, wBottomLeft, wBottomRight),
                new FramePoint(wTopRight, wBottomLeft),
                new FramePoint(wLeft, wTopRight, wBottomRight),
                new FramePoint(wRight, wTopLeft, wBottomLeft),
                new FramePoint(wBottomLeft, wTopRight),
                new FramePoint(wBottom, wTopLeft, wTopRight),
                new FramePoint(wBottomRight, wTopLeft),
            };
        }

        /// <summary>
        /// 枠を描画します
        /// </summary>
        protected override void DrawFrame(Graphics vGraphics) {
            // 枠線は黒色で固定
            using (var wPen = new Pen(C_FrameColor)) {
                vGraphics.DrawRectangle(wPen, this.FrameRectangle);
                foreach (IFramePoint wFramePoint in this.FramePoints) {
                    wFramePoint.Draw(vGraphics, wPen);
                }
            }
        }

        /// <summary>
        /// 拡大・縮小するための座標取得処理
        /// </summary>
        protected override (Point StartPoint, Point EndPoint) GetChangeScalePoints(IFramePoint vFramePoint, Size vSize) {
            var wPoints = vFramePoint.BasePoints.ToList();
            wPoints.Add(vFramePoint.Point + vSize);
            var wStartPoint = new Point(wPoints.Min(p => p.X), wPoints.Min(p => p.Y));
            var wEndPoint = new Point(wPoints.Max(p => p.X), wPoints.Max(p => p.Y));
            return (wStartPoint, wEndPoint);
        }
    }
}