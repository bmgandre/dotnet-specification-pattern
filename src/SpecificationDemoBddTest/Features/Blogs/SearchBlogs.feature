Feature: Search blogs

    Background:
        Given the following blogs exist:
            | BlogId | Url                        | Created    | Activated | Removed | Banned |
            | 1      | http://pcworld.blog.com    | 2017-01-01 |           |         |        |
            | 2      | http://csharpnew.blog.com  | 2015-01-01 |           |         |        |
            | 3      | http://dotnetcore.blog.com | 2017-06-23 |           |         |        |
        And the following posts exist:
            | PostId | BlogId | Title                | Content | Created    | Activated | Removed | Banned |
            | 1      | 1      | Benchmark tools      | Foo bar | 2017-02-01 |           |         |        |
            | 2      | 1      | Overclock principles | Foo bar | 2017-03-01 |           |         |        |

    Scenario Outline: Get blogs not expired and created after date
        Given I looking for blogs created after <date>
        When I search for blogs
        Then the count should be <count>

        Examples:
            | date       | count |
            | 2014-12-25 | 3     |
            | 2015-02-01 | 2     |
            | 2017-03-05 | 1     |