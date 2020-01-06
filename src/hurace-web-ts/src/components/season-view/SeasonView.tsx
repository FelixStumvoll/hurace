import React from 'react';
import { SeasonViewItem } from './SeasonViewItem';
import styled from 'styled-components';
import { Season } from '../../interfaces/Season';
import { getSeasons } from '../../common/api';
import { MasterViewWrapper, SearchContext } from '../shared/MasterViewWrapper';
import { useStateAsync } from '../../hooks/asyncState';

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

export const SeasonView: React.FC = () => {
    const [seasons] = useStateAsync(getSeasons);

    return (
        <MasterViewWrapper
            createText="Saison erstellen"
            createUrl="/season/new"
        >
            <SearchContext.Consumer>
                {searchTerm => (
                    <SeasonItemPanel>
                        {seasons
                            ?.filter(s => seasonFilter(s, searchTerm))
                            .map(s => (
                                <SeasonViewItem key={s.id} season={s} />
                            ))}
                    </SeasonItemPanel>
                )}
            </SearchContext.Consumer>
        </MasterViewWrapper>
    );
};
