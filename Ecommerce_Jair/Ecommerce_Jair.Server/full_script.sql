IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categories] (
    [CategoryID] int NOT NULL IDENTITY,
    [CategoryName] nvarchar(100) NOT NULL,
    [Description] nvarchar(255) NULL,
    [ParentCategoryID] int NULL,
    CONSTRAINT [PK__Categori__19093A2B307D8CCA] PRIMARY KEY ([CategoryID]),
    CONSTRAINT [FK__Categorie__Paren__44FF419A] FOREIGN KEY ([ParentCategoryID]) REFERENCES [Categories] ([CategoryID])
);

CREATE TABLE [DiscountTypes] (
    [TypeName] nvarchar(20) NOT NULL,
    CONSTRAINT [PK__Discount__D4E7DFA90019A970] PRIMARY KEY ([TypeName])
);

CREATE TABLE [OrderStatus] (
    [StatusName] nvarchar(50) NOT NULL,
    CONSTRAINT [PK__OrderSta__05E7698BC377CDE1] PRIMARY KEY ([StatusName])
);

CREATE TABLE [PaymentStatus] (
    [StatusName] nvarchar(50) NOT NULL,
    CONSTRAINT [PK__PaymentS__05E7698BC617CBF5] PRIMARY KEY ([StatusName])
);

CREATE TABLE [Roles] (
    [RoleID] int NOT NULL IDENTITY,
    [RoleName] nvarchar(50) NOT NULL,
    [Description] nvarchar(255) NULL,
    CONSTRAINT [PK__Roles__8AFACE3A5AEEB8E8] PRIMARY KEY ([RoleID])
);

CREATE TABLE [Users] (
    [UserID] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(255) NOT NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [LastLogin] datetime NULL,
    [LastPasswordChange] datetime NULL,
    [PasswordResetToken] nvarchar(255) NULL,
    [IsEmailConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK__Users__1788CCAC7F1BD566] PRIMARY KEY ([UserID])
);

CREATE TABLE [Products] (
    [ProductID] int NOT NULL IDENTITY,
    [ProductName] nvarchar(255) NOT NULL,
    [Description] nvarchar(max) NULL,
    [Price] decimal(18,2) NOT NULL,
    [Stock] int NOT NULL,
    [CategoryID] int NOT NULL,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [UpdatedAt] datetime NULL,
    [SKU] nvarchar(50) NOT NULL,
    CONSTRAINT [PK__Products__B40CC6EDBB16ED13] PRIMARY KEY ([ProductID]),
    CONSTRAINT [FK__Products__Catego__4AB81AF0] FOREIGN KEY ([CategoryID]) REFERENCES [Categories] ([CategoryID])
);

CREATE TABLE [Discounts] (
    [DiscountID] int NOT NULL IDENTITY,
    [Code] nvarchar(50) NOT NULL,
    [Description] nvarchar(255) NULL,
    [DiscountType] nvarchar(20) NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    [ValidFrom] datetime NULL,
    [ValidTo] datetime NULL,
    [IsActive] bit NULL DEFAULT CAST(1 AS bit),
    [CreatedBy] int NULL,
    CONSTRAINT [PK__Discount__E43F6DF69BD4F1D0] PRIMARY KEY ([DiscountID]),
    CONSTRAINT [FK__Discounts__Creat__797309D9] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([UserID]),
    CONSTRAINT [FK__Discounts__Disco__778AC167] FOREIGN KEY ([DiscountType]) REFERENCES [DiscountTypes] ([TypeName])
);

CREATE TABLE [Orders] (
    [OrderID] int NOT NULL IDENTITY,
    [UserID] int NOT NULL,
    [OrderDate] datetime NULL DEFAULT ((getdate())),
    [Status] nvarchar(50) NOT NULL DEFAULT N'Pending',
    [ShippingAddress] nvarchar(255) NOT NULL,
    [PaymentMethod] nvarchar(50) NOT NULL,
    [AdminNotes] nvarchar(max) NULL,
    CONSTRAINT [PK__Orders__C3905BAF19D7417D] PRIMARY KEY ([OrderID]),
    CONSTRAINT [FK__Orders__Status__5535A963] FOREIGN KEY ([Status]) REFERENCES [OrderStatus] ([StatusName]),
    CONSTRAINT [FK__Orders__UserID__52593CB8] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
);

CREATE TABLE [UserRoles] (
    [UserID] int NOT NULL,
    [RoleID] int NOT NULL,
    [IsPrimary] bit NULL DEFAULT CAST(0 AS bit),
    [AssignedDate] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__UserRole__AF27604F302B402E] PRIMARY KEY ([UserID], [RoleID]),
    CONSTRAINT [FK__UserRoles__RoleI__403A8C7D] FOREIGN KEY ([RoleID]) REFERENCES [Roles] ([RoleID]),
    CONSTRAINT [FK__UserRoles__UserI__3F466844] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
);

CREATE TABLE [UserTokens] (
    [ID] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [Token] nvarchar(255) NOT NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [Revoked] bit NOT NULL,
    [TokenType] nvarchar(30) NOT NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserID]) ON DELETE CASCADE
);

CREATE TABLE [ProductReviews] (
    [ReviewID] int NOT NULL IDENTITY,
    [ProductID] int NOT NULL,
    [UserID] int NOT NULL,
    [Rating] int NULL,
    [Comment] nvarchar(max) NULL,
    [CreatedAt] datetime NULL DEFAULT ((getdate())),
    [AdminApproved] bit NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK__ProductR__74BC79AE2B636CD3] PRIMARY KEY ([ReviewID]),
    CONSTRAINT [FK__ProductRe__Produ__693CA210] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]),
    CONSTRAINT [FK__ProductRe__UserI__6A30C649] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
);

CREATE TABLE [ShoppingCart] (
    [CartID] int NOT NULL IDENTITY,
    [UserID] int NOT NULL,
    [ProductID] int NOT NULL,
    [Quantity] int NOT NULL,
    [AddedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Shopping__51BCD797E3650BA9] PRIMARY KEY ([CartID]),
    CONSTRAINT [FK__ShoppingC__Produ__5EBF139D] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]),
    CONSTRAINT [FK__ShoppingC__UserI__5DCAEF64] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
);

CREATE TABLE [Wishlist] (
    [WishlistID] int NOT NULL IDENTITY,
    [UserID] int NOT NULL,
    [ProductID] int NOT NULL,
    [AddedAt] datetime NULL DEFAULT ((getdate())),
    CONSTRAINT [PK__Wishlist__233189CB9D87E2FB] PRIMARY KEY ([WishlistID]),
    CONSTRAINT [FK__Wishlist__Produc__656C112C] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID]),
    CONSTRAINT [FK__Wishlist__UserID__6477ECF3] FOREIGN KEY ([UserID]) REFERENCES [Users] ([UserID])
);

CREATE TABLE [OrderDetails] (
    [OrderDetailID] int NOT NULL IDENTITY,
    [OrderID] int NOT NULL,
    [ProductID] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [TotalPrice] AS ([Quantity]*[UnitPrice]),
    CONSTRAINT [PK__OrderDet__D3B9D30C465ED923] PRIMARY KEY ([OrderDetailID]),
    CONSTRAINT [FK__OrderDeta__Order__5812160E] FOREIGN KEY ([OrderID]) REFERENCES [Orders] ([OrderID]),
    CONSTRAINT [FK__OrderDeta__Produ__59063A47] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([ProductID])
);

CREATE TABLE [Payments] (
    [PaymentID] int NOT NULL IDENTITY,
    [OrderID] int NOT NULL,
    [PaymentDate] datetime NULL DEFAULT ((getdate())),
    [Amount] decimal(18,2) NOT NULL,
    [PaymentMethod] nvarchar(50) NULL,
    [TransactionID] nvarchar(100) NULL,
    [Status] nvarchar(50) NOT NULL,
    [AdminComments] nvarchar(255) NULL,
    CONSTRAINT [PK__Payments__9B556A584806D92F] PRIMARY KEY ([PaymentID]),
    CONSTRAINT [FK__Payments__OrderI__6FE99F9F] FOREIGN KEY ([OrderID]) REFERENCES [Orders] ([OrderID]),
    CONSTRAINT [FK__Payments__Status__71D1E811] FOREIGN KEY ([Status]) REFERENCES [PaymentStatus] ([StatusName])
);

CREATE INDEX [idx_categories_parent] ON [Categories] ([ParentCategoryID]);

CREATE INDEX [IX_Discounts_CreatedBy] ON [Discounts] ([CreatedBy]);

CREATE INDEX [IX_Discounts_DiscountType] ON [Discounts] ([DiscountType]);

CREATE UNIQUE INDEX [UQ__Discount__A25C5AA7A42EC530] ON [Discounts] ([Code]);

CREATE INDEX [IX_OrderDetails_OrderID] ON [OrderDetails] ([OrderID]);

CREATE INDEX [IX_OrderDetails_ProductID] ON [OrderDetails] ([ProductID]);

CREATE INDEX [idx_orders_status] ON [Orders] ([Status]);

CREATE INDEX [idx_orders_userid] ON [Orders] ([UserID]);

CREATE INDEX [idx_payments_status] ON [Payments] ([Status]);

CREATE INDEX [IX_Payments_OrderID] ON [Payments] ([OrderID]);

CREATE INDEX [idx_reviews_product] ON [ProductReviews] ([ProductID]);

CREATE INDEX [IX_ProductReviews_UserID] ON [ProductReviews] ([UserID]);

CREATE INDEX [idx_products_category] ON [Products] ([CategoryID]);

CREATE UNIQUE INDEX [UQ__Products__CA1ECF0D23578401] ON [Products] ([SKU]);

CREATE UNIQUE INDEX [UQ__Roles__8A2B6160613958E0] ON [Roles] ([RoleName]);

CREATE INDEX [IX_ShoppingCart_ProductID] ON [ShoppingCart] ([ProductID]);

CREATE UNIQUE INDEX [UC_ShoppingCart] ON [ShoppingCart] ([UserID], [ProductID]);

CREATE INDEX [IX_UserRoles_RoleID] ON [UserRoles] ([RoleID]);

CREATE INDEX [idx_users_email] ON [Users] ([Email]);

CREATE UNIQUE INDEX [UQ__Users__A9D10534917C79E2] ON [Users] ([Email]);

CREATE INDEX [IX_UserTokens_UserId] ON [UserTokens] ([UserId]);

CREATE INDEX [IX_Wishlist_ProductID] ON [Wishlist] ([ProductID]);

CREATE UNIQUE INDEX [UC_Wishlist] ON [Wishlist] ([UserID], [ProductID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250730181827_InitialCreate', N'9.0.7');

COMMIT;
GO

