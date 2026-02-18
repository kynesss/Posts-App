import {
  Box,
  Stack,
  Card,
  CardContent,
  Typography,
  CircularProgress,
  Alert,
  Pagination,
} from "@mui/material";

import usePosts from "../hooks/usePosts";
import { useState } from "react";

const PostsPage = () => {
  const [pager, setPager] = useState({
    page: 1,
    pageSize: 10,
    sort: "id",
    order: "desc",
  });
  const { posts, isLoading, error } = usePosts(pager);
  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      {error ? (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error}
        </Alert>
      ) : isLoading ? (
        <CircularProgress size={200} color="secondary" />
      ) : (
        <Stack spacing={2} sx={{ width: "100%", maxWidth: 900 }}>
          {posts.items.map((post) => (
            <Box
              key={post.id}
              sx={{
                display: "flex",
                flexDirection: "column",
              }}
            >
              <Card>
                <CardContent>
                  <Typography variant="h6">{post.title}</Typography>
                  <Typography color="text.secondary">{post.content}</Typography>
                </CardContent>
              </Card>
            </Box>
          ))}

          <Box sx={{ display: "flex", justifyContent: "center", pt: 1 }}>
            <Pagination
              page={pager.page}
              count={posts.totalPages ?? 1}
              color="primary"
              onChange={(_, value) =>
                setPager((prev) => ({ ...prev, page: value }))
              }
            />
          </Box>
        </Stack>
      )}
    </Box>
  );
};

export default PostsPage;
