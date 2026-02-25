import {
  Box,
  Stack,
  Card,
  CardContent,
  CardActionArea,
  Typography,
  CircularProgress,
  Alert,
  Pagination,
  TextField,
  Button,
} from "@mui/material";

import usePosts from "../hooks/usePosts";
import { useState } from "react";
import { Link } from "react-router-dom";

const PostsPage = () => {
  const [inputValue, setInputValue] = useState("");
  const [search, setSearch] = useState("");
  const [pager, setPager] = useState({
    page: 1,
    pageSize: 10,
    sort: "id",
    order: "desc",
  });
  const { posts, isLoading, error } = usePosts(search, pager);
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
          <Button
            variant="contained"
            label="test"
            size="large"
            component={Link}
            to="/posts/add"
            sx={{ maxWidth: 150 }}
          >
            Dodaj Post
          </Button>
          <TextField
            label="Szukaj"
            variant="outlined"
            size="small"
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter") {
                setSearch(inputValue);
                setPager((prev) => ({ ...prev, page: 1 }));
              }
            }}
            placeholder="Wpisz tytuł..."
          />
          {posts.items.map((post) => (
            <Box
              key={post.id}
              sx={{
                display: "flex",
                flexDirection: "column",
              }}
            >
              <Card
                sx={{
                  transition: "0.2s",
                  "&:hover": {
                    boxShadow: 6,
                    transform: "translateY(-2px)",
                  },
                }}
              >
                <CardActionArea component={Link} to={`/posts/${post.id}`}>
                  <CardContent>
                    <Typography variant="h6">{post.title}</Typography>
                    <Typography color="text.secondary">
                      {post.content}
                    </Typography>
                  </CardContent>
                </CardActionArea>
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
