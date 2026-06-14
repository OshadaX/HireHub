# HireHub
Job Posting &amp; Application Platform — .NET 8

🍽️ Restaurant
├── 🧑‍🍳 Kitchen      → does the actual cooking      (Domain)
├── 📋 Manager      → decides the rules/recipes     (Application)  
├── 🏗️ Storage      → fridge, suppliers, database   (Infrastructure)
└── 🛎️ Waiter       → talks to customers            (API)

HireHub/
└── src/
    ├── HireHub.Domain/          ← 🧑‍🍳 Kitchen
    │   └── Entities/            
    │       └── User.cs          (just the blueprints, no logic)
    │
    ├── HireHub.Application/     ← 📋 Manager (Phase 5)
    │   └── business rules,
    │       validation logic
    │
    ├── HireHub.Infrastructure/  ← 🏗️ Storage (Phase 2)
    │   └── database, EF Core,
    │       external services
    │
    └── HireHub.API/             ← 🛎️ Waiter (Phase 3)
        └── Controllers/
            HTTP routes, requests