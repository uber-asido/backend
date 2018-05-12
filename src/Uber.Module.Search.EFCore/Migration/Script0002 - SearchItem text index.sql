create index idx_search_item_text on search_item using gin(to_tsvector('english', text));
