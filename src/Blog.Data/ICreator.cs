using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public interface ICreator<T>
    {
        [CanBeNull]
        T CreatorId { get; set; }
    }
}
