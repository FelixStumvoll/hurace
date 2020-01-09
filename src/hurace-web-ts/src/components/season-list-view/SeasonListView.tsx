import React from 'react';
import { SeasonListViewItem } from './SeasonListViewItem';
import styled from 'styled-components';
import { Season } from '../../models/Season';
import { getSeasons } from '../../common/api';
import { ListViewWrapper, SearchContext } from '../shared/ListViewWrapper';
import { useStateAsync } from '../../hooks/useStateAsync';

const SeasonItemPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
    overflow: auto;
`;

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
    const [seasons] = useStateAsync(getSeasons);

    return (
        <ListViewWrapper
            createConfig={{
                createText: 'Saison erstellen',
                createUrl: '/season/new'
            }}
        >
            <SearchContext.Consumer>
                {searchTerm => (
                    <SeasonItemPanel>
                        {seasons
                            ?.filter(s => seasonFilter(s, searchTerm))
                            .map(s => (
                                <SeasonListViewItem key={s.id} season={s} />
                            ))}
                    </SeasonItemPanel>
                )}
            </SearchContext.Consumer>
        </ListViewWrapper>
    );
};
