using FuzzySharp;
using LibraryService.Models;

namespace LibraryService.Queries;

public static class MemberQueries
{
    public static IEnumerable<Member> FuzzySearchByNameAndLastName(IEnumerable<Member> members, string query, int minScore = 50)
    {
        return from member in members 
            let firstNameScore = Fuzz.PartialRatio(query, member.FirstName) 
            let lastNameScore = Fuzz.PartialRatio(query, member.LastName) 
            where firstNameScore >= minScore || lastNameScore >= minScore 
            select member;
    }
}