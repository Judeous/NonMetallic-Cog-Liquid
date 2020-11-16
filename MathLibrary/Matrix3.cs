﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MathLibrary
{
    public class Matrix3
    {
        public float m11, m12, m13, m21, m22, m23, m31, m32, m33;

        public Matrix3()
        {
            m11 = 1; m12 = 0; m13 = 0;
            m21 = 0; m22 = 1; m23 = 0;
            m31 = 0; m32 = 0; m33 = 1;
        } //Constructor

        public Matrix3(
            float m11, float m12, float m13, 
            float m21, float m22, float m23, 
            float m31, float m32, float m33)
        {
            this.m11 = m11; this.m12 = m12; this.m13 = m13;
            this.m21 = m21; this.m22 = m22; this.m23 = m23;
            this.m31 = m31; this.m32 = m32; this.m33 = m33;
        } //Overload constructor

        /// <summary>
        /// Creates a new Matrix rotated by the passed in number of radians
        /// </summary>
        /// <param name="radians">The number of radians the Matrix will be rotated by</param>
        /// <returns></returns>
        public static Matrix3 CreateRotation(float radians)
        {
            return new Matrix3(
                               (float)(Math.Cos(radians)), (float)(Math.Sin(radians)), 0,
                               (float)(-1 * Math.Sin(radians)), (float)(Math.Cos(radians)), 0, 
                               0, 0, 1
                              );
        } //Create Rotation function

        /// <summary>
        /// Creates a new Matrix in the position that was passed in
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Matrix3 CreateTranslation(Vector2 position)
        {
            return new Matrix3(
                               1, 0, position.X,
                               0, 1, position.Y,
                               0, 0, 1);
        } //Create Translation function

        /// <summary>
        /// Creates a new Matrix scaled by the passed in scale
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Matrix3 CreateScale(float xScale, float yScale)
        {
            return new Matrix3(
                               xScale, 0, 0, 
                               0, yScale, 0,
                               0, 0, 1);
        } //Create Scale function

        public static Matrix3 operator +(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m11 + rhs.m11, lhs.m12 + rhs.m12, lhs.m13 + rhs.m13,
                lhs.m21 + rhs.m21, lhs.m22 + rhs.m22, lhs.m23 + rhs.m23,
                lhs.m31 + rhs.m31, lhs.m32 + rhs.m32, lhs.m33 + rhs.m33);
        } //Addition overload

        public static Matrix3 operator -(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m11 - rhs.m11, lhs.m12 - rhs.m12, lhs.m13 - rhs.m13,
                lhs.m21 - rhs.m21, lhs.m22 - rhs.m22, lhs.m23 - rhs.m23,
                lhs.m31 - rhs.m31, lhs.m32 - rhs.m32, lhs.m33 - rhs.m33);
        } //Subtraction overload

        public static Matrix3 operator *(Matrix3 lhs, Matrix3 rhs)
        {
            return new Matrix3(
                lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31, //Row 1 Column 1
                lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32, //Row 1 Column 2
                lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33, //Row 1 Column 3

                lhs.m21* rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31,  //Row 2 Column 1
                lhs.m21* rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32,  //Row 2 Column 2
                lhs.m21* rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33,  //Row 2 Column 3

                lhs.m31* rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31,  //Row 3 Column 1
                lhs.m31* rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32,  //Row 3 Column 2
                lhs.m31* rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33); //Row 3 Column 3
        } //Multiplication overload
    } //Matrix 3
} //Math Library