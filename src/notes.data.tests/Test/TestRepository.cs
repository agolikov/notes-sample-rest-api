﻿using notes.data.Repositories;

namespace notes.data.tests.Test
{
    public class TestRepository : Repository<TestEntity>
    {
        public TestRepository(TestDbContext context) : base(context)
        {
        }
    }
}
