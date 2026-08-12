// Decompiled with JetBrains decompiler
// Type: LOB.Helper.LambdaHelper
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CETAP_LOB.Helper
{
  internal class LambdaHelper
  {
    public static string ToPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
      return ((MemberExpression) propertyExpression.Body).Member.Name;
    }

    private static void DrillIntoExpression(Expression expression, List<string> path)
    {
      if (expression is MemberExpression)
      {
        path.Insert(0, ((MemberExpression) expression).Member.Name);
        LambdaHelper.DrillIntoExpression(((MemberExpression) expression).Expression, path);
      }
      if (!(expression is UnaryExpression))
        return;
      LambdaHelper.DrillIntoExpression(((UnaryExpression) expression).Operand, path);
    }
  }
}
