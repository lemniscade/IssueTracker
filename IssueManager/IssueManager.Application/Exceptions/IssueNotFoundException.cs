using BuildingBlocks.Exceptions;

namespace IssueManager.IssueManager.Application.Exceptions
{
    class IssueNotFoundException(Guid id) : NotFoundException("Issue", id)
    {
    }
}

