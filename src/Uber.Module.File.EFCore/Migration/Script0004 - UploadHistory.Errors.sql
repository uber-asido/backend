alter table upload_history drop column error;
alter table upload_history add column errors text[];
