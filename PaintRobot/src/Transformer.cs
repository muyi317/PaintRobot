using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PaintRobot
{
    class Transformer
    {
        public Matrix<double> m_transformMatrix;
        public double[] m_translation;
        public double[] m_rotation;
        public double[,] m_point1;
        public double[,] m_point2;

        public Transformer(double[,] point1, double[,] point2)
        {
            m_point1 = point1;
            m_point2 = point2;
            generateTransformMatrics();
        }

        public void generateTransformMatrics()
        {
            m_translation = new double[] {
                m_point2[0, 0] - m_point1[0, 0],
                m_point2[0, 1] - m_point1[0, 1],
                m_point2[0, 2] - m_point1[0, 2] };

            m_rotation = new double[] {
                m_point2[1, 0] - m_point1[1, 0],
                m_point2[1, 1] - m_point1[1, 1],
                m_point2[1, 2] - m_point1[1, 2] };

            double Rx = m_rotation[0];
            double Ry = m_rotation[1];
            double Rz = m_rotation[2];

            double x = m_translation[0];
            double y = m_translation[1];
            double z = m_translation[2];

            Matrix<double> RotateMatrixX = DenseMatrix.OfArray(new double[,] {
                {1,0,0,0},
                {0,Math.Cos(Rx),-Math.Sin(Rx),0},
                {0,Math.Sin(Rx),Math.Cos(Rx),0},
                {0,0,0,1},
            });

            Matrix<double> RotateMatrixY = DenseMatrix.OfArray(new double[,] {
                {Math.Cos(Ry),0,Math.Sin(Ry),0},
                {0,1,0,0},
                {-Math.Sin(Ry),0,Math.Cos(Ry),0},
                {0,0,0,1},
            });

            Matrix<double> RotateMatrixZ = DenseMatrix.OfArray(new double[,] {
                {Math.Cos(Rz),-Math.Sin(Rz),0,0},
                {Math.Sin(Rz),Math.Cos(Rz),0,0},
                {0,0,1,0},
                {0,0,0,1},
            });

            Matrix<double> TransferMatrix = DenseMatrix.OfArray(new double[,] {
                {0,0,0,x},
                {0,0,0,y},
                {0,0,0,z},
                {0,0,0,0},
            });

            m_transformMatrix = RotateMatrixX * RotateMatrixY * RotateMatrixZ + TransferMatrix;
            Console.WriteLine(m_transformMatrix);
        }

        public double[] calculateDst(double[] src)
        {
            Matrix<double> homogeneousSrc = DenseMatrix.OfArray(new double[,] { { src[0] }, { src[1] }, { src[2] }, { 1 } });

            Matrix<double> transformation1 = DenseMatrix.OfArray(new double[,] { { m_point2[0, 0] }, { m_point2[0, 1] }, { m_point2[0, 2] }, { 0 } });

            homogeneousSrc = homogeneousSrc - transformation1;

            double[,] homogeneousDst = (m_transformMatrix * homogeneousSrc + transformation1).ToArray();

            double[] dst = { homogeneousDst[0, 0], homogeneousDst[1, 0], homogeneousDst[2, 0] };

            return dst;
        }
    }
}
