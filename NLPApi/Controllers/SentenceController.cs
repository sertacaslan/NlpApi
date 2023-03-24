using Microsoft.AspNetCore.Mvc;
using NLPApi.Models;
using System.Text.Json;

namespace NLPApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SentenceController : ControllerBase
    {
        private readonly NLP _nlp;

        public SentenceController(NLP nlp)
        {
            _nlp = nlp;
        }

        [HttpPost(Name = "SendSentence")]
        public IActionResult SendSentence([FromBody] TextModel text)
        {
            string sonuc= _nlp.Converter(text.UserText);

            ResultModel result = new ResultModel { Output = sonuc };


            return new JsonResult(result);
        }
    }
}