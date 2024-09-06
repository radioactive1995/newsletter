using ErrorOr;
using System.Text.Json.Serialization;

namespace Domain.Common;

//Define a result object that will either contain the value or an error message
//Expand on this, so that it also supports list of errors as a discrimante type
//This will allow us to return multiple errors in a single response
//This will also allow us to return a list of errors in a single response
//LIST OF ERROR
//public class Result<T> where T : class
//{
//    public T? Value { get; }
//    public Error? Error { get; }
//    public bool IsSuccess => !Error.HasValue;
//
//    public bool IsError => Error.HasValue;
//
//    [JsonConstructor]
//    public Result(T? value = default, Error? error = default)
//    {
//        Value = value;
//        Error = error;
//    }
//
//    public Result(T value)
//    {
//        Value = value;
//    }
//
//    public Result(Error error)
//    {
//        Error = error;
//    }
//
//    public static implicit operator Result<T>(T value) => new(value);
//    public static implicit operator Result<T>(Error error) => new(error);
//}

public record struct Result<T>
{
    public T? Value { get; }
    public Error FirstError => Errors.FirstOrDefault();
    public List<Error> Errors { get; }
    public bool IsSuccess => !IsError;

    public bool IsError => Errors.Count > 0;

    [JsonConstructor]
    public Result(T? value = default, List<Error>? errors = default)
    {
        Value = value;
        Errors = errors ?? new();
    }

    public Result(T value)
    {
        Value = value;
        Errors = [];
    }

    public Result(List<Error> errors)
    {
        Value = default(T);
        Errors = errors;
    }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(List<Error> errors) => new(errors);
    public static implicit operator Result<T>(Error error) => new([ error ]);
}

