﻿namespace ReformedAIO
{
    #region Using Directives

    using System;

    using LeagueSharp;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Color = System.Drawing.Color;

    #endregion

    internal class HpBarIndicator
    {
        #region Static Fields

        public static Device DxDevice = Drawing.Direct3DDevice;

        public static Line DxLine;

        #endregion

        #region Fields

        public float Hight = 9;

        public float Width = 104;

        #endregion

        #region Constructors and Destructors

        public HpBarIndicator()
        {
            DxLine = new Line(DxDevice) { Width = 9 };

            Drawing.OnPreReset += DrawingOnOnPreReset;
            Drawing.OnPostReset += DrawingOnOnPostReset;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainOnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnDomainUnload;
        }

        #endregion

        #region Public Properties

        public Vector2 StartPosition
            => new Vector2(Unit.HPBarPosition.X + Offset.X, Unit.HPBarPosition.Y + Offset.Y);

        public Obj_AI_Hero Unit { get; set; }

        #endregion

        #region Properties

        private Vector2 Offset
        {
            get
            {
                if (Unit != null)
                {
                    return Unit.IsAlly ? new Vector2(34, 9) : new Vector2(10, 20);
                }

                return new Vector2();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void DrawDmg(float dmg, ColorBGRA color)
        {
            var hpPosNow = GetHpPosAfterDmg(0);
            var hpPosAfter = GetHpPosAfterDmg(dmg);

            FillHpBar(hpPosNow, hpPosAfter, color);
            //fillHPBar((int)(hpPosNow.X - startPosition.X), (int)(hpPosAfter.X- startPosition.X), color);
        }

        #endregion

        #region Methods

        private static void CurrentDomainOnDomainUnload(object sender, EventArgs eventArgs)
        {
            DxLine.Dispose();
        }

        private static void DrawingOnOnPostReset(EventArgs args)
        {
            DxLine.OnResetDevice();
        }

        private static void DrawingOnOnPreReset(EventArgs args)
        {
            DxLine.OnLostDevice();
        }

        private void FillHpBar(int to, int from, Color color)
        {
            var sPos = StartPosition;
            for (var i = from; i < to; i++)
            {
                Drawing.DrawLine(sPos.X + i, sPos.Y, sPos.X + i, sPos.Y + 9, 1, color);
            }
        }

        private void FillHpBar(Vector2 from, Vector2 to, ColorBGRA color)
        {
            DxLine.Begin();

            DxLine.Draw(
                new[] { new Vector2((int)from.X, (int)from.Y + 4f), new Vector2((int)to.X, (int)to.Y + 4f) },
                color);

            DxLine.End();
        }

        private Vector2 GetHpPosAfterDmg(float dmg)
        {
            var w = GetHpProc(dmg) * Width;
            return new Vector2(StartPosition.X + w, StartPosition.Y);
        }

        private float GetHpProc(float dmg = 0)
        {
            var health = Unit.Health - dmg > 0 ? Unit.Health - dmg : 0;
            return health / Unit.MaxHealth;
        }

        #endregion
    }
}