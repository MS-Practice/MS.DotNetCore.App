﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Logging.Demo.Core.Models
{
    public class Character
    {
        public Character(string name)
        {
            Name = name;
        }

        private Character()
        {
        }

        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; } = String.Empty;
    }
}
