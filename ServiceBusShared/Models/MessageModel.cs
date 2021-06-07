using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceBusShared.Models
{
    public class MessageModel
    {
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
