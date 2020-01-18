import React from 'react';
import { SeasonListViewItem } from './SeasonListViewItem';
import { Season } from '../../models/Season';
import { getSeasons } from '../../common/api';
import { ListViewWrapper, SearchContext } from '../shared/ListViewWrapper';
import { FlexWrap } from '../../theme/CustomComponents';
import { useAsync } from 'react-async-hook';
import { LoadingWrapper } from '../shared/LoadingWrapper';
import { compareSeasons } from '../../common/compareFunctions';

const seasonFilter = (season: Season, searchTerm: string) =>
    season.startDate
        .getFullYear()
        .toString()
        .toLowerCase()
        .includes(searchTerm.toLowerCase()) ||
    season.endDate
        .getFullYear()
        .toString()
        .toLowerCase()
        .includes(searchTerm.toLowerCase());

export const SeasonListView: React.FC = () => {
    const { error, loading, result: seasons } = useAsync(getSeasons, []);

    return (
        <LoadingWrapper loading={loading} error={error}>
            <ListViewWrapper
                createConfig={{
                    createText: 'Saison erstellen',
                    createUrl: '/season/new'
                }}
            >
                <SearchContext.Consumer>
                    {searchTerm => (
                        <FlexWrap>
                            {seasons
                                ?.filter(s => seasonFilter(s, searchTerm))
                                .sort(compareSeasons)
                                .map(s => (
                                    <SeasonListViewItem key={s.id} season={s} />
                                ))}
                        </FlexWrap>
                    )}
                </SearchContext.Consumer>
            </ListViewWrapper>
        </LoadingWrapper>
    );
};
